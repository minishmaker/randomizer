.thumb
pop	{r3}
add	r0,r1
ldrh	r0,[r0]
str	r3,[sp]
ldr	r3,=#0x80A2A80
mov	lr,r3
.short	0xF800

@check if we won
mov	r0,r9
add	r0,#0x70
loop:
ldrb	r1,[r0]
cmp	r1,#0
beq	end
cmp	r1,#0xFF
beq	won
add	r0,#1
b	loop

won:
mov	r0,r9
add	r0,#0x68
mov	r1,#0
strh	r1,[r0]

end:
ldr	r3,=#0x80A0B31
bx	r3
