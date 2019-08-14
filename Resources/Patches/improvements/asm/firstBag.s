.thumb
ldr	r3,=#0x8053B88
mov	lr,r3
.short	0xF800
mov	r1,r4
add	r1,#0x68

ldrb	r2,[r4,#10]
cmp	r2,#0x65
bne	end
ldr	r3,=#0x2002AEE
ldrb	r2,[r3]
cmp	r2,#0
bne	end
mov	r2,#1
strb	r2,[r3]
sub	r3,#2
mov	r2,#10
strb	r2,[r3]

end:
ldr	r3,=#0x8083619
bx	r3
