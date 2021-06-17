.thumb
push	{r4-r5, lr}
mov	r4, r0
mov	r5, r1

@ check if r5 is the link room event
ldr	r0, =#0x8009E84
cmp	r0, r5
bne	end

@ check link's coords
ldr	r0, =#0x3001160
ldr	r1, =#0x7FFF0000
ldr	r2, [r0, #0x2C]
cmp	r2, r1
blo	end
ldr	r2, [r0, #0x30]
cmp	r2, r1
blo	end

@ set the new event
ldr	r5, =#0x8009EB8

@ set new quickwarp point to normal room quickwarp
ldr	r0, =#0x2002AC8
ldr	r1, spawnPoint
mov	r2, #0
spawnPointLoop:
	ldrb	r3, [r1, r2]
	strb	r3, [r0, r2]
	add	r2, #1
	cmp	r2, #8
bne	spawnPointLoop

@ make the screen black
ldr	r0, =#0x3000FD0
mov	r1, #1
strb	r1, [r0, #0x00]
mov	r1, #5
strh	r1, [r0, #0x08]
mov	r1, #0
strh	r1, [r0, #0x0C]

end:
mov	r0, r4
mov	r1, #0x24
ldr	r3, =#0x807D5B4
mov	lr, r3
.short	0xF800

.align
.ltorg
spawnPoint:
