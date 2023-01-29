.thumb
@check if in-game
ldr	r0,=#0x3001002
ldrb	r0,[r0]
cmp	r0,#2
blo	end
cmp	r0,#3
bhi	end
ldr	r0,=#0x200012C
ldr	r0,[r0]
cmp	r0,#0
beq	end

@check if vaati has been defeated
ldr	r0,=#0x2002CA6
ldrb	r0,[r0]
mov	r1,#2
and	r0,r1
cmp	r0,#0
bne	end

@increase global timer
ldr	r2,=#0x203FFF0
ldr	r0,[r2]
add	r0,#1
str	r0,[r2]

@check if menu
ldr	r0,=#0x200008C
ldr	r0,[r0]
cmp	r0,#0
beq	notMenu
@increase menu timer
ldr	r0,[r2,#4]
add	r0,#1
str	r0,[r2,#4]
b	save


notMenu:
@check if cutscene
ldr	r0,=#0x3003DC0
ldr	r0,[r0]
cmp	r0,#0
beq	notCutscene
@increase cutscene timer
ldr	r0,[r2,#8]
add	r0,#1
str	r0,[r2,#8]
b	save

notCutscene:
@check if rolling
ldr	r0,=#0x300116C
ldrb	r0,[r0]
cmp	r0,#0x18
bne	save
@increase rolling timer
ldr	r0,[r2,#12]
add	r0,#1
str	r0,[r2,#12]

save:
ldr	r0,=#0x2000004
ldrb	r0,[r0]
ldr	r1,=#0x500
mul	r0,r1
ldr	r3,=#0xE0015F0
add	r0,r3
mov	r3,#0
saveLoop:
ldrb	r1,[r2,r3]
strb	r1,[r0,r3]
add	r3,#1
cmp	r3,#16
bne	saveLoop

end:
ldr	r1,=#0x8055AE8
ldr	r1,[r1]
ldrb	r0,[r6,#2]
lsl	r0,#2
add	r0,r1
ldr	r0,[r0]
ldr	r3,=#0x8055AD1
bx	r3
.align
.ltorg
