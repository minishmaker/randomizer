.thumb
cmp	r4,#0x17
bne	notOcarina
ldr	r0,=#0x300116C
ldrb	r0,[r0]
cmp	r0,#1
bne	end
notOcarina:
mov	r0,r4
ldr	r3,=#0x8077378
mov	lr,r3
.short	0xF800
end:
mov	r1,r0
ldr	r3,=#0x80772E1
bx	r3
