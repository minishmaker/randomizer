.thumb
ldrb	r0,[r4,#0x0A]
ldrb	r1,[r4,#0x0B]
mov	r2,#0
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800

@set a flag based on area
ldr	r0,=#0x3000BF0
ldrb	r0,[r0,#4]
cmp	r0,#5
beq	shoot
cmp	r0,#4
beq	dig
cmp	r0,#9
beq	swim
b	end

shoot:
mov	r1,#27
b	set
dig:
mov	r1,#28
b	set
swim:
mov	r1,#29
b	set

set:
ldr	r0,=#0x2002EA4
ldr	r3,=#0x801D5F4	@vanilla flag set routine
mov	lr,r3
.short	0xF800

end:
ldr	r3,=#0x809F2F7
bx	r3
